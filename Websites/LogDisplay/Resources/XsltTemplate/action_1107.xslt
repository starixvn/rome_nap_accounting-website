<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_customerupddisablehr">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          {lbl:disabledHR}
        </td>
        <td class="colW2">
          <xsl:value-of select="@disablehorseracing" />
        </td>
        <td class="colW1">
          {lbl:isUpdateDownline}
        </td>
        <td class="colW2">
          <xsl:value-of select="@disabledownline" />
        </td>


        <td class="colW1">
          {lbl:language}
        </td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
      </tr>

      <tr>
        <td class="colW1">
          {lbl:errCode}
        </td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1">
          {lbl:errMsg}
        </td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>

        <td class="colW1">
          {lbl:ip}
        </td>
        <td class="colW2">
          <xsl:value-of select="@ip" />
        </td>
      </tr>


      <xsl:choose>
        <xsl:when test="$IsInternal = 0">
         
        </xsl:when>
        <xsl:otherwise>
          <tr>
            <xsl:choose>
              <xsl:when test="@subaccname">
                <td class="colW1">{lbl:subaccname}</td>
                <td class="colW2">
                  <xsl:value-of select="@subaccname" />
                </td>
              </xsl:when>
              <xsl:otherwise>
                <td class="colW1">{lbl:subAccID}</td>
                <td class="colW2">
                  <xsl:value-of select="@subaccid" />
                </td>
               </xsl:otherwise>
            </xsl:choose>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>
        </xsl:otherwise>
      </xsl:choose>
     

    </table>
  </xsl:template>
</xsl:stylesheet>
