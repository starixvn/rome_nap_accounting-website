<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdmovemember">
        <table class="tblDesc">
            <tr>
                <td class="colW1">{lbl:MemberName}</td>
                <td class="colW2">
                    <xsl:value-of select="@membername" />
                </td>

              <xsl:choose>
                <xsl:when test="@newagentname">
                  <td class="colW1">{lbl:NewAgentName}</td>
                  <td class="colW2">
                    <xsl:value-of select="@newagentname" />
                  </td>
                </xsl:when>
                <xsl:when test="@agentname">
                  <td class="colW1">{lbl:AgentName}</td>
                  <td class="colW2">
                    <xsl:value-of select="@agentname" />
                  </td>
                </xsl:when>
              </xsl:choose>
              <td class="colW1">{lbl:ReturnErrCode}</td>
              <td class="colW2">
                <xsl:value-of select="@returnerrcode" />
              </td> 
              
                
            </tr>
            <tr>
              <td class="colW1">{lbl:ErrCode}</td>
              <td class="colW2">
                <xsl:value-of select="@errcode" />
              </td>
              <td class="colW1">{lbl:ErrMsg}</td>
              <td class="colW2">
                <xsl:value-of select="@errmsg" />
              </td>
              <xsl:choose>
                <xsl:when test="@oldagentid">
                  <td class="colW1">{lbl:OldAgentId}</td>
                  <td class="colW2">
                    <xsl:value-of select="@oldagentid" />
                  </td>
                </xsl:when>
                <xsl:otherwise>
                  <td class="colW1"></td>
                  <td class="colW2">
                  </td>
                </xsl:otherwise>
              </xsl:choose>

            </tr>

          <xsl:choose>
            <xsl:when test="@newagentid">
              <tr>
                <td class="colW1">{lbl:NewAgentId}</td>
                <td class="colW2">
                  <xsl:value-of select="@newagentid" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">
                </td>
                <td class="colW1"></td>
                <td class="colW2">
                </td>
              </tr>
            </xsl:when>
           
          </xsl:choose>
          
          
        </table>









    </xsl:template>
</xsl:stylesheet>
