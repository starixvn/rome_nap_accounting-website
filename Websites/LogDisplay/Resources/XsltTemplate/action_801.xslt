<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdvirtualmultiplier">
        <table class="tblDesc">
            <tr>
              <xsl:choose>
                <xsl:when test="@newvalue">
                  <td class="colW1">{lbl:NewValue}</td>
                  <td class="colW2">
                    <xsl:value-of select="@newvalue" />
                  </td>  
                </xsl:when>
                <xsl:when test="@virtualmultiplier">
                  <td class="colW1">{lbl:VirtualMultiplier}</td>
                  <td class="colW2">
                    <xsl:value-of select="@virtualmultiplier" />
                  </td>
                </xsl:when>
              </xsl:choose>
                  
                
              
                <td class="colW1">{lbl:OldValue}</td>
                <td class="colW2">
                    <xsl:value-of select="@oldvalue" />
                </td>
              <td class="colW1">{lbl:ErrCode}</td>
              <td class="colW2">
                <xsl:value-of select="@errcode" />
              </td>
            </tr>
            <tr>
              <td class="colW1">{lbl:ErrMsg}</td>
              <td class="colW2">
                <xsl:value-of select="@errmsg" />
              </td>
              <xsl:choose>
                <xsl:when test="@ip">
                  <td class="colW1">{lbl:IP}</td>
                  <td class="colW2">
                    <xsl:value-of select="@ip" />
                  </td>    
                </xsl:when>
                <xsl:otherwise>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </xsl:otherwise>
              </xsl:choose>
              
               
                
                <td class="colW1"></td>
                <td class="colW2">

                </td>
            </tr>
            <tr>

            </tr>
        </table>





    </xsl:template>
</xsl:stylesheet>
